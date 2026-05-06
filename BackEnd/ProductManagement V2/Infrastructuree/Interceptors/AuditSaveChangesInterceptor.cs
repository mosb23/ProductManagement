using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductManagement_V2.Application.Common.Auth;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Infrastructuree.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;

        public AuditSaveChangesInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditFields(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditableEntity auditable)
                {
                    if (entry.State == EntityState.Added && !HasExplicitCreatedAudit(auditable))
                    {
                        auditable.SetCreated(now, _currentUserService.DisplayName, _currentUserService.UserId);
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditable.SetUpdated(now, _currentUserService.DisplayName, _currentUserService.UserId);
                    }

                    if (entry.State == EntityState.Deleted && entry.Entity is SoftDeleteEntity softDelete)
                    {
                        entry.State = EntityState.Modified;
                        softDelete.MarkAsDeleted(now);
                    }
                }
            }
        }

        private static bool HasExplicitCreatedAudit(AuditableEntity auditable)
        {
            return !string.IsNullOrWhiteSpace(auditable.CreatedByUserId)
                || auditable.CreatedBy != "System";
        }
    }
}
