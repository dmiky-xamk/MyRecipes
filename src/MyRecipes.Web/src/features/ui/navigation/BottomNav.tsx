import { Book, Search, ShoppingCart } from "@mui/icons-material";
import { BottomNavigation, BottomNavigationAction, Paper } from "@mui/material";
import React, { useEffect } from "react";
import { Link as RouterLink, useLocation } from "react-router-dom";

export default function BottomNav() {
  const { pathname } = useLocation();
  const [value, setValue] = React.useState(pathname);

  // Update the bottom navigation if the user navigates with the browser.
  useEffect(() => {
    setValue(pathname);
  }, [pathname]);

  return (
    <Paper
      sx={{ position: "sticky", bottom: 0, marginTop: "auto" }}
      elevation={3}
    >
      <BottomNavigation
        showLabels
        value={value}
        onChange={(_, newValue) => {
          setValue(newValue);
        }}
      >
        <BottomNavigationAction
          value="/"
          component={RouterLink}
          to="/"
          icon={<Book />}
          label="Recipes"
        />
        <BottomNavigationAction
          value="/search"
          component={RouterLink}
          to="search"
          icon={<Search />}
          label="Search"
        />
        <BottomNavigationAction
          value="/cart"
          component={RouterLink}
          to="cart"
          icon={<ShoppingCart />}
          label="Cart"
        />
      </BottomNavigation>
    </Paper>
  );
}
