import React, { useEffect, useState } from 'react';
import { GetPermissionResponse, GetPermissionsResponse } from '../../types';
import permissionsService from '../../services/permissionsService';
import PermissionsPageContainer from './presentational';
import { toast } from 'react-toastify';

const PermissionsPage: React.FC = () => {
  const [permissions, setPermissions] = useState<GetPermissionsResponse>({ totalCount: 0, permissions: [] });
  const [loading, setLoading] = useState<boolean>(true);
  const [page, setPage] = useState<number>(0);
  const [pageSize, setPageSize] = useState<number>(10);

  useEffect(() => {
    const fetchPermissions = async () => {
      setLoading(true);
      try {
        const data = await permissionsService.GetAllAsync(page + 1, pageSize);
        setPermissions(data);
      } catch (error) {
        console.error('Failed to fetch permissions:', error);
        toast.error('Failed to fetch permissions');
      } finally {
        setLoading(false);
      }
    };

    fetchPermissions();
  }, [page, pageSize]);

  const handleAddPermission = async (newPermission: GetPermissionResponse) => {
    try {
      await permissionsService.RequestAsync(newPermission);
      const data = await permissionsService.GetAllAsync(page + 1, pageSize);
      setPermissions(data);
      toast.success('Permission added successfully');
    } catch (error) {
      console.error('Failed to add permission:', error);
      toast.error('Failed to add permission');
    }
  };

  const handleEditPermission = async (updatedPermission: GetPermissionResponse) => {
    try {
      await permissionsService.ModifyAsync(updatedPermission);
      const data = await permissionsService.GetAllAsync(page + 1, pageSize);
      setPermissions(data);
      toast.success('Permission updated successfully');
    } catch (error) {
      console.error('Failed to update permission:', error);
      toast.error('Failed to update permission');
    }
  };

  const handleDeletePermission = async (permissionId: number) => {
    try {
      await permissionsService.DeleteAsync(permissionId);
      const data = await permissionsService.GetAllAsync(page + 1, pageSize);
      setPermissions(data);
      toast.success('Permission deleted successfully');
    } catch (error) {
      console.error('Failed to delete permission:', error);
      toast.error('Failed to delete permission');
    }
  };

  const handlePageChange = (event: React.ChangeEvent<unknown>, newPage: number) => {
    setPage(newPage);
  };

  const handlePageSizeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newSize = parseInt(event.target.value, 10);
    setPageSize(newSize);
    setPage(0);
  };

  return (
    <>
      <PermissionsPageContainer
        data={permissions.permissions}
        totalCount={permissions.totalCount}
        loading={loading}
        onAdd={handleAddPermission}
        onEdit={handleEditPermission}
        onDelete={handleDeletePermission}
        page={page}
        pageSize={pageSize}
        onPageChange={handlePageChange}
        onPageSizeChange={handlePageSizeChange}
      />
    </>
  );
};

export default PermissionsPage;
