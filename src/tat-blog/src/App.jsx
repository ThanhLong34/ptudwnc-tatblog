import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import "./styles/App.css";

import Footer from "./components/Footer";
import Layout from "./pages/Layout";
import Index from "./pages/Index";
import About from "./pages/About";
import Contact from "./pages/Contact";
import RSS from "./pages/RSS";

import AdminLayout from "./pages/admin/AdminLayout";
import * as AdminIndex from './pages/admin/Index';

function App() {
	return (
		<Router>
			<Routes>
				<Route path="/" element={<Layout />}>
					<Route path="/" element={<Index />} />
					<Route path="blog" element={<Index />} />
					<Route path="blog/Contact" element={<Contact />} />
					<Route path="blog/About" element={<About />} />
					<Route path="blog/RSS" element={<RSS />} />
				</Route>
				<Route path="/admin" element={<AdminLayout />}>
					<Route path="/admin" element={<AdminIndex.default />} />
				</Route>
			</Routes>

			<Footer />
		</Router>
	);
}
export default App;
