import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Paper,
    CircularProgress,
    Button,
    Alert
} from '@mui/material';
import { GetPermissionResponse } from '../../types';
import { PermissionModal } from '../../components';
import permissionsService from '../../services/permissionsService';
import { useNavigate } from 'react-router-dom';

interface PermissionDetailPageContainerProps {
    permission: GetPermissionResponse | null;
    loading: boolean;
    error: string | null;
    onPermissionUpdated: (updatedPermission: GetPermissionResponse) => void;
}

const PermissionDetailPageContainer: React.FC<PermissionDetailPageContainerProps> = ({ permission, loading, error, onPermissionUpdated }) => {
    const [isEditDialogOpen, setIsEditDialogOpen] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [editPermission, setEditPermission] = useState<GetPermissionResponse | null>(permission);
    const navigate = useNavigate();

    useEffect(() => {
        if (error) {
            navigate('/permissions');
        }
    }, [error, navigate]);

    const handleOpenEditDialog = () => {
        setEditPermission(permission); // Reset form state to the current permission data
        setIsEditDialogOpen(true);
    };

    const handleCloseEditDialog = () => {
        setIsEditDialogOpen(false);
    };

    const handleEditPermission = async (updatedPermission: GetPermissionResponse) => {
        setIsSubmitting(true);
        try {
            await permissionsService.ModifyAsync(updatedPermission);
            onPermissionUpdated(updatedPermission);
            handleCloseEditDialog();
        } catch (error) {
            console.error('Failed to update permission:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleBackToPermissions = () => {
        navigate('/permissions');
    };

    if (loading) return <CircularProgress />;
    if (error) return <Alert severity="error">{error}</Alert>;
    if (!permission) return <Typography>No permission data found.</Typography>;

    return (
        <Container>
            <Typography variant="h4" gutterBottom>
                Permission Details
            </Typography>
            <Paper sx={{ p: 3, mt: 2 }}>
                <Typography variant="h6">Permission ID: {permission.permissionId}</Typography>
                <Typography variant="body1">Employee Name: {permission.employeeName}</Typography>
                <Typography variant="body1">Employee Surname: {permission.employeeSurname}</Typography>
                <Typography variant="body1">Permission Type: {permission.permissionTypeDescription}</Typography>
                <Typography variant="body1">Created: {new Date(permission.created).toLocaleString()}</Typography>
                <Typography variant="body1">Last Modified: {new Date(permission.lastModified).toLocaleString()}</Typography>
            </Paper>
            <Button
                variant="contained"
                color="primary"
                sx={{ mt: 2, mr: 2 }}
                onClick={handleOpenEditDialog}
            >
                Edit Permission
            </Button>
            <Button
                variant="outlined"
                color="secondary"
                sx={{ mt: 2 }}
                onClick={handleBackToPermissions}
            >
                Back to Permissions
            </Button>

            {editPermission && (
                <PermissionModal
                    open={isEditDialogOpen}
                    onClose={handleCloseEditDialog}
                    onSubmit={() => handleEditPermission(editPermission)} // Submit handler
                    isSubmitting={isSubmitting}
                    dialogType="edit"
                    permission={editPermission} // Pass the form state
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                        const { name, value } = event.target;
                        setEditPermission(prev => prev ? { ...prev, [name]: value } : null); // Update form state
                    }}
                />
            )}
        </Container>
    );
};

export default PermissionDetailPageContainer;
