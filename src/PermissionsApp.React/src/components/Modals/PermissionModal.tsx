import React, { useEffect, useState } from 'react';
import {
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
    Button,
    CircularProgress,
    FormControl,
    InputLabel,
    Select,
    MenuItem
} from '@mui/material';
import { GetPermissionResponse, GetPermissionTypeResponse } from '../../types';
import permissionsTypeService from '../../services/permissionsTypeService';

interface PermissionDialogProps {
    open: boolean;
    onClose: () => void;
    onSubmit: () => void;
    isSubmitting: boolean;
    dialogType: 'add' | 'edit';
    permission: GetPermissionResponse;
    onChange: (event: React.ChangeEvent<HTMLInputElement | { name?: string; value: unknown }>) => void;
}

const PermissionDialog: React.FC<PermissionDialogProps> = ({
    open,
    onClose,
    onSubmit,
    isSubmitting,
    dialogType,
    permission,
    onChange
}) => {
    const [permissionTypes, setPermissionTypes] = useState<GetPermissionTypeResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchPermissionTypes = async () => {
            try {
                const data = await permissionsTypeService.GetAllAsync();
                setPermissionTypes(data);
            } catch (error) {
                console.error('Failed to fetch permission types:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchPermissionTypes();
    }, []);

    // Set a default value if permission.permissionTypeId is not set
    const defaultPermissionTypeId = permission.permissionTypeId || permissionTypes[0]?.permissionTypeId || '';

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>{dialogType === 'add' ? 'Add Permission' : 'Edit Permission'}</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    name="employeeName"
                    label="Employee Name"
                    type="text"
                    fullWidth
                    value={permission.employeeName}
                    onChange={onChange}
                />
                <TextField
                    margin="dense"
                    name="employeeSurname"
                    label="Employee Surname"
                    type="text"
                    fullWidth
                    value={permission.employeeSurname}
                    onChange={onChange}
                />
                <FormControl fullWidth margin="dense" disabled={loading}>
                    <InputLabel id="permission-type-label">Permission Type</InputLabel>
                    <Select
                        labelId="permission-type-label"
                        name="permissionTypeId"
                        value={permission.permissionTypeId || defaultPermissionTypeId}
                        onChange={onChange}
                    >
                        {permissionTypes.map((type) => (
                            <MenuItem key={type.permissionTypeId} value={type.permissionTypeId}>
                                {type.description}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="primary">
                    Cancel
                </Button>
                <Button onClick={onSubmit} color="primary" disabled={isSubmitting}>
                    {isSubmitting ? <CircularProgress size={24} /> : 'Submit'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default PermissionDialog;
