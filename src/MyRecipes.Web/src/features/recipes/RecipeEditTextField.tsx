import { styled, TextField } from "@mui/material";

const RecipeEditTextField = styled(TextField)({
  variant: "outlined",
  backgroundColor: "#fff",
});

const FormHelperTextStyle = {
  style: {
    backgroundColor: "#f7f7f7",
    margin: 0,
    paddingLeft: 10,
  },
};

export { RecipeEditTextField, FormHelperTextStyle };
