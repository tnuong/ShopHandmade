import { FC, useEffect, useState } from "react";
import BlogItem from "../components/BlogItem";
import { Breadcrumb } from "antd";
import { BlogResource } from "../../../resources";
import blogService from "../../../services/blog-service";
import GridBlogSkeleton from "../../shared/skeleton/GridBlogSkeleton";

const HomeBlog: FC = () => {
    const [loading, setLoading] = useState(false)
    const [blogs, setBlogs] = useState<BlogResource[]>([])

    const fetchBlogs = async () => {
        const response = await blogService.getMostRecentPosts(1, 3);
        setBlogs(response.data)
    }

    useEffect(() => {
        setLoading(true)
        fetchBlogs();
        setLoading(false)
    }, [])

    return <div className="max-w-screen-xl w-full mx-auto flex flex-col items-start md:items-center gap-y-8">
        <h2 className="text-2xl md:text-3xl font-bold text-center mb-10">
            Các bài viết mới nhất
        </h2>
        {loading ?
            <GridBlogSkeleton />
            :
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                {blogs.map(blog => <BlogItem key={blog.id} blog={blog} />)}
            </div>
        }
    </div>
};

export default HomeBlog;
