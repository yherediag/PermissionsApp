import GetPermissionResponse from "./GetPermissionResponse";

interface GetPermissionsResponse {
  totalCount: number;
  permissions: GetPermissionResponse[];
}

export default GetPermissionsResponse;