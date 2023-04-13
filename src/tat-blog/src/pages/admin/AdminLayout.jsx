import { Outlet } from "react-router-dom";
import Navbar from "../../components/admin/Navbar";
import Footer from "../../components/Footer";

const AdminLayout = () => {
	return (
		<>
			<Navbar />
			<div className="container-fluid py-3">
				<Outlet />
			</div>
		</>
	);
};
export default AdminLayout;
