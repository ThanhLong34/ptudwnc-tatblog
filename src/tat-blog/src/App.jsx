import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import "./styles/App.css";

import Navbar from "./components/Navbar";
import Sidebar from "./components/Sidebar";
import Footer from "./components/Footer";

function App() {
	return (
		<div>
			<Router>
				<Navbar />
				<div className="container-fluid">
					<div className="row">
						<div className="col-9"></div>
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
