import { createBrowserRouter } from "react-router-dom";
import { AppLayout, Error, Home } from "../LazyComponents";

export const routes = [
  {
    element: <AppLayout />,
    errorElement: <Error />,
    children: [
      {
        path: "/",
        element: <Home />,
        errorElement: <Error />,
      },
    ],
  },
];

export const router = createBrowserRouter(routes);
