import axiosClient from "../config/axiosClient";
import {
  GetPermissionsResponse,
  GetPermissionResponse,
  ModifyPermissionCommand,
  RequestPermissionCommand,
} from "../types";

// Fetch all permissions
export async function GetAllAsync(
  pageNumber?: number,
  pageSize?: number
): Promise<GetPermissionsResponse> {
  try {
    const { data } = await axiosClient.get<GetPermissionsResponse>(
      "/permissions",
      {
        params: { pageNumber, pageSize },
      }
    );
    return data;
  } catch (error) {
    console.error("Error fetching permissions:", error);
    throw error;
  }
}

export async function GetAsync(
  permissionId: number
): Promise<GetPermissionResponse> {
  try {
    const { data } = await axiosClient.get<GetPermissionResponse>(
      `/permissions/${permissionId}`
    );
    return data;
  } catch (error) {
    console.error(`Error fetching permission with ID ${permissionId}:`, error);
    throw error;
  }
}

export async function RequestAsync(
  newPermission: RequestPermissionCommand
): Promise<void> {
  try {
    await axiosClient.post("/permissions", newPermission);
  } catch (error) {
    console.error("Error adding permission:", error);
    throw error;
  }
}

export async function ModifyAsync(
  updatedPermission: ModifyPermissionCommand
): Promise<void> {
  try {
    await axiosClient.put(
      `/permissions/${updatedPermission.permissionId}`,
      updatedPermission
    );
  } catch (error) {
    console.error("Error updating permission:", error);
    throw error;
  }
}

export async function DeleteAsync(permissionId: number): Promise<void> {
  try {
    await axiosClient.delete(`/permissions/${permissionId}`);
  } catch (error) {
    console.error("Error deleting permission:", error);
    throw error;
  }
}

export default {
  GetAllAsync,
  GetAsync,
  RequestAsync,
  ModifyAsync,
  DeleteAsync,
};
