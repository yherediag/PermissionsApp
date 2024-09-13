import React, { useState } from 'react';
import {
    AppBar,
    Toolbar,
    Typography,
    Container,
    Box,
    CssBaseline,
    Button,
    IconButton,
    useMediaQuery,
    useTheme,
    Drawer,
    List,
    ListItem,
    ListItemText,
    Divider
} from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { Outlet, Link } from 'react-router-dom';

const MainLayoutContainer: React.FC = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const [drawerOpen, setDrawerOpen] = useState<boolean>(false);

    const toggleDrawer = (open: boolean) => () => {
        setDrawerOpen(open);
    };

    const drawerItems = (
        <Box
            sx={{ width: 250 }}
            role="presentation"
            onClick={toggleDrawer(false)}
            onKeyDown={toggleDrawer(false)}
        >
            <List>
                <ListItem component={Link} to="/permissions">
                    <ListItemText primary="Permissions" />
                </ListItem>
                <Divider />
            </List>
        </Box>
    );

    return (
        <Box>
            <CssBaseline />
            <AppBar position="static">
                <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    {isMobile && (
                        <IconButton
                            edge="start"
                            color="inherit"
                            aria-label="menu"
                            onClick={toggleDrawer(true)}
                        >
                            <MenuIcon />
                        </IconButton>
                    )}
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <Typography variant="h6" sx={{ mr: 10 }}>
                            N5 - PermissionsApp
                        </Typography>
                        {!isMobile && (
                            <Button color="inherit" component={Link} to="/permissions">
                                Permissions
                            </Button>
                        )}
                    </Box>
                </Toolbar>
            </AppBar>
            <Drawer
                anchor="left"
                open={drawerOpen}
                onClose={toggleDrawer(false)}
            >
                {drawerItems}
            </Drawer>
            <Container sx={{ mt: 2 }}>
                <Outlet />
            </Container>
        </Box>
    );
};

export default MainLayoutContainer;
