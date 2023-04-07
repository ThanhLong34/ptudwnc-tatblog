import { Link } from "react-router-dom";
import Card from "react-bootstrap/Card";
import { isEmptyOrSpaces } from "../Utils/utils";
import TagList from "./TagList";

const PostList = ({ postItem }) => {
	let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
		? "/images/no-image.jpg"
		: `${postItem.imageUrl}`;

	let postedDate = new Date(postItem.postedDate);

	return (
		<article className="blog-entry mb-4">
			<Card>
				<div className="row g-0">
					<div className="col-md-4"></div>
					<Card.Img className="post-image" variant="top" src={imageUrl} alt={postItem.title} />
				</div>
				<div className="col-md-8">
					<Card.Body>
						<Card.Title>
							<Link to={`/post/${postedDate.getFullYear()}/${postedDate.getMonth() + 1}/${postedDate.getDate()}/${postItem.urlSlug}`}>{postItem.title}</Link>
						</Card.Title>
						<Card.Text>
							<small className="text-muted">Tác giả:</small>
							<span className="text-primary m-1">
								<Link to={`/author/${postItem.author.urlSlug}`}>{postItem.author.fullname}</Link>
							</span>
							<small className="text-muted">Chủ đề:</small>
							<span className="text-primary m-1">
								<Link to={`/author/${postItem.category.urlSlug}`}>{postItem.category.name}</Link>
							</span>
						</Card.Text>
						<Card.Text>{postItem.shortDescription}</Card.Text>
						<div className="tag-list">
							<TagList tagList={postItem.tags} />
						</div>
						<div className="text-end">
							<Link
								to={`/blog/post?year=${postedDate.getFullYear()}&month=${postedDate.getMonth()}&day=${postedDate.getDay()}&slug=${
									postItem.urlSlug
								}`}
								className="btn btn-primary"
								title={postItem.title}
							>
								Xem chi tiết
							</Link>
						</div>
					</Card.Body>
				</div>
			</Card>
		</article>
	);
};

export default PostList;
