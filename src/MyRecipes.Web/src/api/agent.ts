import axios, { AxiosError, AxiosResponse } from "axios";
import { AuthCredentials, AuthResponse } from "../features/auth/auth";
import { Recipe } from "../features/recipes/recipe";
import { TestUserRecipes } from "./testUserApi";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.headers.common["Content-Type"] = "application/json";

const responseBody = (response: AxiosResponse) => response.data;

export const updateAxiosToken = (token: string | null) =>
  (axios.defaults.headers.common.Authorization = `Bearer ${token}`);

// Use the test user API instead of the real API.
export const enableTestUser = () => {
  Recipes.list = TestUserRecipes.list;
  Recipes.details = TestUserRecipes.details;
  Recipes.update = TestUserRecipes.update;
  Recipes.create = TestUserRecipes.create;
  Recipes.delete = TestUserRecipes.delete;
};

export interface ApiErrorResponse {
  status: number;
  title: string;
  traceId: string;
  type: string;
}

const sleep = (delay: number) =>
  new Promise((resolve) => setTimeout(resolve, delay));

axios.interceptors.response.use(
  async (response) => {
    if (process.env.NODE_ENV === "development") {
      await sleep(1000);
    }

    return response;
  },
  async (error: AxiosError<ApiErrorResponse>) => {
    await sleep(1000);
    const { data, status } = error.response!;

    // TODO: Manage string errors?
    // Data error can be a string: Proxy error: Could not proxy request /account/login from localhost:3000 to https://localhost:7150/api/v1 (ECONNREFUSED)
    // Or an object (Api error response)
    /*
      status: 400
      title: "Invalid credentials"
      traceId: "00-e62085b6a35eb047a1522e5a266eff3f-62a9bba98deafcd7-00"
      type: "https://tools.ietf.org/html/rfc7231#section-6.5.1"
    */

    return Promise.reject(data);
  }
);

const requests = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  delete: (url: string) => axios.delete(url).then(responseBody),
};

const Recipes = {
  list: (): Promise<Recipe[]> => requests.get("/recipes"),
  details: (id: string): Promise<Recipe> => requests.get(`/recipes/${id}`),
  update: (id: string, recipe: Recipe) =>
    requests.put(`/recipes/${id}`, recipe),
  create: (recipe: Recipe): Promise<Recipe> =>
    requests.post(`/recipes`, recipe),
  delete: (id: string) => requests.delete(`/recipes/${id}`),
};

const Account = {
  user: (): Promise<AuthResponse> => requests.get("/account"),
  login: (user: AuthCredentials): Promise<AuthResponse> =>
    requests.post("/account/login", user),
  register: (user: AuthCredentials): Promise<AuthResponse> =>
    requests.post("/account/register", user),
};

const agent = {
  Account,
  Recipes,
};

export default agent;
