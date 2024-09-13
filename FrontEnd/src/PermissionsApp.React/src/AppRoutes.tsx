import { Route, Routes } from "react-router-dom";
import { MainLayout } from "./layouts";
import { PermissionDetailPage, PermissionsPage } from "./pages";

const AppRoutes = () => {
    return (
        <Routes>
            <Route path="/" element={<MainLayout />}>
                <Route index element={<PermissionsPage />} />
                <Route path="/permissions" element={<PermissionsPage />} />
                <Route path="/permissions/:id" element={<PermissionDetailPage />} />
            </Route>
        </Routes>
    );
};

export default AppRoutes;