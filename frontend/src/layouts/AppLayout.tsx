import { Outlet } from "react-router-dom";

function AppLayout() {
  return (
    <div>
      <div>My React App</div>
      <Outlet />
    </div>
  );
}

export default AppLayout;
