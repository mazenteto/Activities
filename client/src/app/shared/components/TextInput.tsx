import { TextField, type TextFieldProps } from "@mui/material";
import {
  useController,
  useFormContext,
  type FieldValues,
  type UseControllerProps,
} from "react-hook-form";

type Props<T extends FieldValues> = {} & UseControllerProps<T> & TextFieldProps;

export default function TextInput<T extends FieldValues>({
  control,
  ...props
}: Props<T>) {
  const formContext = useFormContext<T>();
  const effictiveControl = control || formContext.control;
  if(!effictiveControl){
    throw new Error('Text input must be used within a form provider or pass as props')
  }

  const { field, fieldState } = useController({ ...props,control:effictiveControl });

  return (
    <TextField
      {...props}
      {...field}
      value={field.value || ""}
      fullWidth
      variant="outlined"
      error={!!fieldState.error}
      helperText={fieldState.error?.message}
    />
  );
}
