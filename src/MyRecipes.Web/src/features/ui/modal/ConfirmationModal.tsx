import {
  Box,
  Button,
  Modal as MuiModal,
  Stack,
  Typography,
} from "@mui/material";

interface Props {
  title: string;
  description: string;
  isOpen: boolean;
  isPending: boolean;
  onConfirm: () => void;
  handleClose: () => void;
}

export default function ConfirmationModal({
  title,
  description,
  isOpen,
  isPending,
  onConfirm,
  handleClose,
}: Props) {
  return (
    <MuiModal
      open={isOpen}
      onClose={handleClose}
      aria-labelledby="modal-title"
      aria-describedby="modal-description"
    >
      <Box
        position="absolute"
        top="50%"
        left="50%"
        width="80%"
        maxWidth="500px"
        borderRadius={2}
        p={3}
        bgcolor="background.paper"
        sx={{ transform: "translate(-50%, -50%)" }}
      >
        <Typography id="modal-title" variant="h6" component="h2" mb={1}>
          {title}
        </Typography>
        <Typography id="modal-description" mb={2}>
          {description}
        </Typography>
        <Stack direction="row" gap={2}>
          <Button
            variant="contained"
            color="error"
            disabled={isPending}
            onClick={onConfirm}
          >
            Delete
          </Button>
          <Button
            variant="outlined"
            color="error"
            disabled={isPending}
            onClick={handleClose}
          >
            Cancel
          </Button>
        </Stack>
      </Box>
    </MuiModal>
  );
}
