import { Button, styled, type ButtonProps } from "@mui/material";
import type { LinkProps } from "react-router";
type StyledButtinPrpos=ButtonProps& Partial <LinkProps>
const StyledButton=styled(Button)<StyledButtinPrpos>(({theme})=>({
    '&.Mui-disabled':{
        backgroundColor: theme.palette.grey[600],
        color:theme.palette.text.disabled
    }
}))
export default StyledButton;