import React, { useEffect, useState } from "react";
import PostList from "../components/PostList";

const Index = () => {
	const [postList, setPostList] = useState([]);

	useEffect(() => {
		document.title = "Trang chủ";
	}, []);

	if (postList.length > 0)
		return (
			<div className="p-4">
				{postList.map((item) => (
					<PostList postItem={item} />
				))}
			</div>
		);
	else return <></>;
};

export default Index;
