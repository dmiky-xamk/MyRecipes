import { Container, SxProps, Theme } from "@mui/material";
import { ReactNode } from "react";

interface Props {
  children: ReactNode;
  maxWidth?: "xs" | "sm" | "md" | "lg" | "xl";
  center?: boolean;
  fullHeight?: boolean;
  sx?: SxProps<Theme>;
}

const centerSx = {
  display: "flex",
  flexDirection: "column",
  justifyContent: "center",
  alignItems: "center",
  textAlign: "center",
};

export default function PageContainer({
  children,
  maxWidth = "md",
  center = false,
  fullHeight = false,
  sx,
}: Props) {
  const centerContent = center === true ? centerSx : {};
  const height = fullHeight ? "100%" : undefined;

  return (
    <Container
      component="main"
      maxWidth={maxWidth}
      sx={{ mt: 4, ...sx, ...centerContent, height }}
    >
      {children}
    </Container>
  );
}
