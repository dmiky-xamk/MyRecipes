import { useCreateRecipe } from "../../features/recipes/recipe";
import RecipeForm from "../../features/recipes/RecipeForm";
import PageContainer from "../../features/ui/main/PageContainer";

export default function CreateRecipe() {
  const mutate = useCreateRecipe();

  return (
    <PageContainer sx={{ mt: 0, p: 0 }}>
      <RecipeForm mutate={mutate} />
    </PageContainer>
  );
}
