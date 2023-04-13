import { Link } from 'react-router-dom'

function NotFound() {
  return (
	  <div>
		  <h2>404</h2>
		  <h4>Không tìm thấy trang</h4>
		  <h4>Trang mà bạn đang tìm không tồn tại</h4>
		  <Link to={'/'}>Về trang chủ</Link>
	 </div>
  )
}

export default NotFound
