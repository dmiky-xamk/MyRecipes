import {
  Checkbox,
  Divider,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import { Fragment } from "react";

export default function CartListItem({
  ingredient,
  checked,
  onCheck,
}: {
  ingredient: string;
  checked: string[];
  onCheck: React.Dispatch<React.SetStateAction<string[]>>;
}) {
  const handleToggle = (value: string) => () => {
    const currentIndex = checked.indexOf(value);
    const newChecked = [...checked];

    if (currentIndex === -1) {
      newChecked.push(value);
    } else {
      newChecked.splice(currentIndex, 1);
    }

    onCheck(newChecked);
  };

  return (
    <Fragment>
      <ListItem disablePadding>
        <ListItemButton role="undefined" onClick={handleToggle(ingredient)}>
          <ListItemIcon>
            <Checkbox
              edge="start"
              checked={checked.indexOf(ingredient) !== -1}
              tabIndex={-1}
              disableRipple
              inputProps={{
                "aria-labelledby": `checkbox-list-label-${ingredient}`,
              }}
            />
          </ListItemIcon>
          <ListItemText primary={ingredient} />
        </ListItemButton>
      </ListItem>
      <Divider component="li" />
    </Fragment>
  );
}
