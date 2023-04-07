import React, { useEffect } from "react";

const Contact = () => {
	useEffect(() => {
		document.title = "Contact";
	}, []);
	return <h1>Đây là contact</h1>;
};

export default Contact;
