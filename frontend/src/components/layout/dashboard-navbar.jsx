import { useLocation, Link } from "react-router-dom";
import {
  Navbar,
  Typography,
  IconButton,
  Breadcrumbs,
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
  Avatar,
} from "@material-tailwind/react";
import {
  BellIcon,
  ClockIcon,
  Bars3Icon,
  UserIcon,
} from "@heroicons/react/24/solid";
import {
  useMaterialTailwindController,
  setOpenSidenav,
  setOpenUserInfo,
} from "@/context";

export function DashboardNavbar() {
  const [controller, dispatch] = useMaterialTailwindController();
  const { fixedNavbar, openSidenav } = controller;
  const { pathname } = useLocation();

  // Dividir y generar rutas acumulativas
  const pathParts = pathname.split("/").filter((el) => el !== "");
  const breadcrumbs = pathParts.map((part, index) => {
    const route = `/${pathParts.slice(0, index + 1).join("/")}`;
    const isLast = index === pathParts.length - 1;

    // Reminder: Handle navigation logic
    // return isLast ? (
    //   <Typography
    //     key={route}
    //     variant="small"
    //     // color="blue-gray"
    //     className="font-normal opacity-50 transition-all hover:text-blue-500 hover:opacity-100"
    //   >
    //     {part}
    //   </Typography>
    // ) : (
    //   <Link key={route} to={route}>
    //     <Typography
    //       variant="small"
    //       color="blue-gray"
    //       className="font-normal opacity-50 transition-all hover:text-blue-500 hover:opacity-100"
    //     >
    //       {part}
    //     </Typography>
    //   </Link>
    // );
  });

  return (
    <Navbar
      color={fixedNavbar ? "white" : "transparent"}
      className={`rounded-xl transition-all ${
        fixedNavbar
          ? "sticky top-4 z-40 py-3 shadow-md shadow-blue-gray-500/5"
          : "px-0 py-1"
      }`}
      fullWidth
      blurred={fixedNavbar}
    >
      <div className="flex flex-col-reverse justify-between gap-6 md:flex-row md:items-center">
        <div className="capitalize">
          <Breadcrumbs
            className={`bg-transparent p-0 transition-all ${
              fixedNavbar ? "mt-1" : ""
            }`}
          >
            {breadcrumbs}
          </Breadcrumbs>
          <Typography variant="h6" color="blue-gray">
            {pathParts[pathParts.length - 1]}
          </Typography>
        </div>
        <div className="flex items-center">
          {/* Botón para abrir el sidebar */}
          <IconButton
            variant="text"
            color="blue-gray"
            className="grid xl:hidden"
            onClick={() => setOpenSidenav(dispatch, !openSidenav)}
          >
            <Bars3Icon strokeWidth={3} className="h-6 w-6 text-blue-gray-500" />
          </IconButton>
          
          {/* Información del usuario */}
          <IconButton
            variant="text"
            color="blue-gray"
            onClick={() => setOpenUserInfo(dispatch, true)}
          >
            <UserIcon className="h-5 w-5 text-blue-gray-500" />
          </IconButton>
        </div>
      </div>
    </Navbar>
  );
}

DashboardNavbar.displayName = "/src/widgets/layout/dashboard-navbar.jsx";

export default DashboardNavbar;
