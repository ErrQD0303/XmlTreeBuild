import { useRouteError } from "react-router-dom";
import { isErrorResponse, isTypeError } from "../shared/helpers/errors";

function Error() {
  const error = useRouteError();

  let errorMessage: string | undefined;
  if (isErrorResponse(error)) errorMessage = error.data;
  else if (isTypeError(error)) errorMessage = error.message;
  else errorMessage = "Unexpected error";

  return (
    <div>
      <div>Error</div>
      <div>{errorMessage}</div>
    </div>
  );
}

export default Error;
