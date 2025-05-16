import { FC, useState } from "react";
import Logo from "../../areas/shared/Logo";
import { Link } from "react-router-dom";
import NavbarRight from "../shared/NavbarRight";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";
import { Avatar, Button, Divider, Drawer, Modal, Popconfirm } from "antd";
import { useDispatch, useSelector } from "react-redux";
import { selectAuth, signOut } from "../../feature/auth/authSlice";
import { AppDispatch } from "../../app/store";
import useModal from "../../hooks/useModal";
import AuthModal from "../../areas/customers/components/modals/AuthModal";

export type ModalAuthType = 'login' | 'register' | 'forgot';

const Header: FC = () => {
    const [open, setOpen] = useState(false);
    const { isAuthenticated, user } = useSelector(selectAuth)
    const dispatch = useDispatch<AppDispatch>()
    const { handleCancel, handleOk, isModalOpen, showModal } = useModal();
    const [modalAuthType, setModalAuthType] = useState<ModalAuthType>('login')

    const handleLogout = () => {
        dispatch(signOut())
        handleOk()
    }

    const handleOpenPopup = (typeModal: ModalAuthType) => {
        setModalAuthType(typeModal)
        showModal()
    }

    const showDrawer = () => {
        setOpen(true);
    };

    const onClose = () => {
        setOpen(false);
    };


    const accountHeader = (): JSX.Element => {
        return <div className="flex items-center gap-x-3 px-4">
            <Avatar className="w-16 h-16" src={user?.avatar} size='large' />
            <div className="flex flex-col">
                <span className="font-semibold text-[16px]">{user?.name}</span>
                <span className="text-[14px]">{user?.roles.includes('ADMIN') ? 'Quản trị viên' : 'Khách hàng'}</span>
            </div>
        </div>
    }

    return <div className={`z-10 flex items-center justify-between min-h-24`}>
        <FontAwesomeIcon onClick={showDrawer} className="block lg:hidden text-2xl cursor-pointer" icon={faBars} />
        <Logo />
        <div className="lg:flex gap-x-8 hidden text-black font-semibold">
            <Link to="/" className="text-lg">Trang chủ</Link>
            <Link to="/shop" className="text-lg">Cửa hàng</Link>
            <Link to="/blog" className="text-lg">Bài viết</Link>
        </div>
        <NavbarRight />

        <Drawer placement="left" title={isAuthenticated ? accountHeader() : <Logo />} onClose={onClose} open={open}>
            <div className="flex flex-col gap-y-4 text-black font-semibold">
                <Link to="/" className="text-lg">Trang chủ</Link>
                <Link to="/shop" className="text-lg">Cửa hàng</Link>
                <Link to="/blog" className="text-lg">Bài viết</Link>
            </div>
            <Divider />
            {isAuthenticated ? <Popconfirm
                className="mt-4"
                placement="bottom"
                title={'Đăng xuất'}
                description={'Bạn có chắc muốn đăng xuất không!'}
                okText="Chắc chắn"
                cancelText="Không, hủy bỏ"
                onConfirm={() => handleLogout()}
            >
                <Button type="default">Đăng xuất</Button>
            </Popconfirm> : <div className="flex gap-x-2">
                <Button onClick={() => handleOpenPopup('login')} type="primary">Đăng nhập</Button>
                <Button onClick={() => handleOpenPopup('register')} type="default">Đăng kí</Button>
            </div>}

        </Drawer>

        <Modal
            open={isModalOpen}
            onOk={handleOk}
            onCancel={handleCancel}
            style={{
                top: 10
            }}
            width={modalAuthType === 'register' ? 700 : 500}
            footer={[]}
        >
            <AuthModal modalType={modalAuthType} handleOk={handleOk} />
        </Modal>
    </div>
};


export default Header;
