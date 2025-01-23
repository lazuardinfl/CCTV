import { env } from "$env/dynamic/private";
import { SvelteKitAuth } from "@auth/sveltekit";
import Keycloak from "@auth/sveltekit/providers/keycloak";

declare module "@auth/core/jwt" {
    interface JWT {
        accessToken?: string | null
        idToken?: string | null
    }
}

export const { handle, signIn, signOut } = SvelteKitAuth({
    debug: env.NODE_ENV !== "production",
    trustHost: true,
    providers: [Keycloak],
    callbacks: {
        async jwt({ account, token }) {
            if (account) {
                token.accessToken = account.access_token;
                token.idToken = account.id_token;
            }
            return token;
        }
    },
    events: {
        async signOut(message) {
            const queries = new URLSearchParams({
                "client_id": env.AUTH_KEYCLOAK_ID,
                "id_token_hint": (message as any).token.idToken
            });
            await fetch(`${env.AUTH_KEYCLOAK_ISSUER}/protocol/openid-connect/logout?${queries.toString()}`, {
                method: "GET"
            });
        }
    }
});
