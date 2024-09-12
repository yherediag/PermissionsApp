import React, { useState } from 'react';
import {
  Container,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  CircularProgress,
  IconButton,
  TablePagination,
} from '@mui/material';
import { Edit, Delete, Visibility } from '@mui/icons-material';
import { GetPermissionResponse } from '../../types';
import { PermissionModal } from '../../components/index';
import { useNavigate } from 'react-router-dom';

interface PermissionsPageContainerProps {
  data: GetPermissionResponse[];
  totalCount: number;
  loading: boolean;
  onAdd: (permission: GetPermissionResponse) => void;
  onEdit: (permission: GetPermissionResponse) => void;
  onDelete: (permissionId: number) => void;
  page: number;
  pageSize: number;
  onPageChange: (event: React.ChangeEvent<unknown>, newPage: number) => void;
  onPageSizeChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

const PermissionsPageContainer: React.FC<PermissionsPageContainerProps> = ({
  data,
  totalCount,
  loading,
  onAdd,
  onEdit,
  onDelete,
  page,
  pageSize,
  onPageChange,
  onPageSizeChange,
}) => {
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [editPermission, setEditPermission] = useState<GetPermissionResponse | null>(null);
  const [newPermission, setNewPermission] = useState<GetPermissionResponse>({
    permissionId: 0,
    employeeName: '',
    employeeSurname: '',
    permissionTypeId: 0,
    permissionTypeDescription: '',
    created: new Date().toISOString(),
    lastModified: new Date().toISOString()
  });
  const [dialogType, setDialogType] = useState<'add' | 'edit'>('add');
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const navigate = useNavigate();

  const handleOpenDialog = (type: 'add' | 'edit', permission?: GetPermissionResponse) => {
    setDialogType(type);
    if (type === 'edit' && permission) {
      setEditPermission(permission);
      setNewPermission(permission);
    } else {
      setNewPermission({
        permissionId: 0,
        employeeName: '',
        employeeSurname: '',
        permissionTypeId: 0,
        permissionTypeDescription: '',
        created: new Date().toISOString(),
        lastModified: new Date().toISOString()
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditPermission(null);
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setNewPermission((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    setIsSubmitting(true);
    try {
      if (dialogType === 'add') {
        await onAdd(newPermission);
      } else if (dialogType === 'edit' && editPermission) {
        await onEdit({ ...editPermission, ...newPermission });
      }
      handleCloseDialog();
    } catch (error) {
      console.error('Error submitting form:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleNavigateToDetail = (permissionId: number) => {
    navigate(`/permissions/${permissionId}`);
  };

  if (loading) return <CircularProgress />;

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Permissions
      </Typography>
      <Button variant="contained" color="primary" onClick={() => handleOpenDialog('add')}>
        Add Permission
      </Button>
      <TableContainer component={Paper} sx={{ mt: 2 }}>
        <Table sx={{ minWidth: 650, tableLayout: 'fixed' }}>
          <TableHead>
            <TableRow>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Permission ID</TableCell>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Employee Name</TableCell>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Employee Surname</TableCell>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Permission Type</TableCell>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Created</TableCell>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Last Modified</TableCell>
              <TableCell sx={{ p: 1, fontSize: '0.875rem' }}>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {data.map((permission) => (
              <TableRow key={permission.permissionId}>
                <TableCell sx={{ p: 1, fontSize: '0.75rem' }}>{permission.permissionId}</TableCell>
                <TableCell sx={{ p: 1, fontSize: '0.75rem' }}>{permission.employeeName}</TableCell>
                <TableCell sx={{ p: 1, fontSize: '0.75rem' }}>{permission.employeeSurname}</TableCell>
                <TableCell sx={{ p: 1, fontSize: '0.75rem' }}>{permission.permissionTypeDescription}</TableCell>
                <TableCell sx={{ p: 1, fontSize: '0.75rem' }}>{new Date(permission.created).toLocaleString()}</TableCell>
                <TableCell sx={{ p: 1, fontSize: '0.75rem' }}>{new Date(permission.lastModified).toLocaleString()}</TableCell>
                <TableCell sx={{ p: 1 }}>
                  <IconButton onClick={() => handleNavigateToDetail(permission.permissionId)} size="small">
                    <Visibility fontSize="small" />
                  </IconButton>
                  <IconButton onClick={() => handleOpenDialog('edit', permission)} size="small">
                    <Edit fontSize="small" />
                  </IconButton>
                  <IconButton onClick={() => onDelete(permission.permissionId)} size="small">
                    <Delete fontSize="small" />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <TablePagination
        rowsPerPageOptions={[10, 25, 50]}
        component="div"
        count={totalCount}
        rowsPerPage={pageSize}
        page={page}
        onPageChange={onPageChange}
        onRowsPerPageChange={onPageSizeChange}
      />

      <PermissionModal
        open={openDialog}
        onClose={handleCloseDialog}
        onSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        dialogType={dialogType}
        permission={newPermission}
        onChange={handleChange}
      />
    </Container>
  );
};

export default PermissionsPageContainer;
