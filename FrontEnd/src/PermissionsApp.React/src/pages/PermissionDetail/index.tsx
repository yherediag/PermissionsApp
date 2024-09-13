import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { GetPermissionResponse } from '../../types';
import permissionsService from '../../services/permissionsService';
import PermissionDetailPageContainer from './presentational';
import { CircularProgress, Typography } from '@mui/material';
import { toast } from 'react-toastify';

const PermissionDetailPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [permission, setPermission] = useState<GetPermissionResponse | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchPermission = async () => {
            setLoading(true);
            try {
                const data = await permissionsService.GetAsync(Number(id));
                setPermission(data);
            } catch (err) {
                setError('Failed to load permission data.');
                console.error('Error fetching permission:', err);
                toast.error('Failed to load permission data');
            } finally {
                setLoading(false);
            }
        };

        fetchPermission();
    }, [id]);

    const handlePermissionUpdated = (updatedPermission: GetPermissionResponse) => {
        setPermission(updatedPermission);
        toast.success('Permission updated successfully');
    };

    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;

    return (
        <PermissionDetailPageContainer
            permission={permission}
            loading={loading}
            onPermissionUpdated={handlePermissionUpdated}
            error={null}
        />
    );
};

export default PermissionDetailPage;
