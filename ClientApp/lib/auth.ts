import { SvelteKitAuth } from "@auth/sveltekit";
import Keycloak from "@auth/sveltekit/providers/keycloak";

declare module "@auth/core/jwt" {
    interface JWT {
        accessToken?: string | null
    }
}

export const { handle, signIn, signOut } = SvelteKitAuth({
    debug: true,
    providers: [Keycloak],
    callbacks: {
        async jwt({ token, account }) {
            if (account) {
                token.accessToken = account.access_token;
            }
            return token;
        }
    }
});
