import { Link } from "react-router-dom";
import { useQuery } from "../Utils/utils";

const BadRequest = () => {
	let query = useQuery();
	let redirectTo = query.get("redirectTo") ?? "/";
	return <>
		<div></div>
	</>;
};

export default BadRequest;
