import axios from "axios";

export async function getCategories(keyword = "", pageSize = 10, pageNumber = 1) {
	try {
		const params = new URLSearchParams({
			PageSize: pageSize,
			PageNumber: pageNumber,
		})

		const response = await axios.get(`https://localhost:7217/api/categories?${params}`);
		const data = response.data;
		if (data.isSuccess) return data.result;
		else return null;
	} catch (error) {
		console.log("Error", error.message);
		return null;
	}
}
