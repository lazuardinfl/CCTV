import { env } from "$env/dynamic/private";
import { decode } from "@auth/core/jwt";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async (event) => {
    const session = await event.locals.auth();
    const jwt = await decode({
        salt: "authjs.session-token",
        secret: env.AUTH_SECRET,
        token: event.cookies.get("authjs.session-token")
    });
    console.log(jwt?.accessToken);
    return {
        session
    };
}
