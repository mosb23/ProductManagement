using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Infrastructuree.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private string GetCurrentUser()
        {
            return "Admin";
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
            var currentUser = GetCurrentUser();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditableEntity auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.SetCreated(now, currentUser);
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditable.SetUpdated(now, currentUser);
                    }

                    if (entry.State == EntityState.Deleted && entry.Entity is SoftDeleteEntity softDelete)
                    {
                        entry.State = EntityState.Modified;
                        softDelete.MarkAsDeleted(now);
                    }
                }
            }
        }
    }
}
