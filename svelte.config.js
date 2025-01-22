import adapter from "@sveltejs/adapter-node";
import { vitePreprocess } from "@sveltejs/vite-plugin-svelte";
import { loadEnv } from "vite";

const env = loadEnv(process.env.NODE_ENV ?? "development", process.cwd(), "SVELTE_");

/** @type {import('@sveltejs/kit').Config} */
const config = {
	// Consult https://svelte.dev/docs/kit/integrations
	// for more information about preprocessors
	preprocess: vitePreprocess(),

	kit: {
		// adapter-auto only supports some environments, see https://svelte.dev/docs/kit/adapter-auto for a list.
		// If your environment is not supported, or you settled on a specific environment, switch out the adapter.
		// See https://svelte.dev/docs/kit/adapters for more information about adapters.
		adapter: adapter(),
		files: {
			assets: "ClientApp/static",
			hooks: {
				client: "ClientApp/hooks.client",
				server: "ClientApp/hooks.server",
				universal: "ClientApp/hooks"
			},
			lib: "ClientApp/lib",
			params: "ClientApp/params",
			routes: "ClientApp/routes",
			serviceWorker: "ClientApp/service-worker",
			appTemplate: "ClientApp/app.html",
			errorTemplate: "ClientApp/error.html"
		},
		paths: {
			base: env.SVELTE_PATH_BASE ?? ""
		}
	}
};

export default config;
