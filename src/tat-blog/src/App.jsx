import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import "./styles/App.css";

import Navbar from "./components/Navbar";
import Sidebar from "./components/Sidebar";
import Footer from "./components/Footer";

import Layout from "./pages/Layout";
import Index from "./pages/Index";
import About from "./pages/About";
import Contact from "./pages/Contact";
import RSS from "./pages/RSS";

function App() {
	return (
		<div>
			<Router>
				<Navbar />
				<div className="container-fluid">
					<div className="row">
						<div className="col-9">
							<Routes>
								<Route path="/" element={<Layout />}>
									<Route path="/" element={<Index />} />
									<Route path="blog" element={<Index />} />
									<Route path="blog/Contact" element={<Contact />} />
									<Route path="blog/About" element={<About />} />
									<Route path="blog/RSS" element={<RSS />} />
								</Route>
							</Routes>
						</div>
						<div className="col-3 border-start">
							<Sidebar />
						</div>
					</div>
				</div>
				<Footer />
			</Router>
		</div>
	);
}
export default App;
