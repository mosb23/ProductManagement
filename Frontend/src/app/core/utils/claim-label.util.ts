export function getClaimLabel(claim: string): string {
  const map: Record<string, string> = {
    ProductsView: 'View Products',
    ProductsCreate: 'Create Products',
    ProductsDelete: 'Delete Products',
    ProductsChangeStatus: 'Change Product Status',

    UsersView: 'View Users',
    UsersCreate: 'Create Users',

    StatisticsView: 'View Statistics',

    ProductStatusHistoriesView: 'View Product Status History',

    RolesView: 'View Roles'
  };

  return map[claim] || claim;
}