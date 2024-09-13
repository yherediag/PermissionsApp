import axiosClient from "../config/axiosClient";
import { GetPermissionTypeDto } from "../types";

// Fetch all permissions type
export async function GetAllAsync(): Promise<GetPermissionTypeDto[]> {
  try {
    const { data } = await axiosClient.get<GetPermissionTypeDto[]>('/permissionsType');
    return data;
  } catch (error) {
    console.error('Error fetching permissions type:', error);
    throw error;
  }
}

export default {
  GetAllAsync,
};