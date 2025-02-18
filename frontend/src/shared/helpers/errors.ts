import { ErrorResponse } from "react-router-dom";

export const isErrorResponse = (error: unknown): error is ErrorResponse =>
  (error as ErrorResponse).data !== undefined;

export const isTypeError = (error: unknown): error is TypeError =>
  (error as TypeError).message !== undefined;
