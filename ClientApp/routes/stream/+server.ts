import { env } from "$env/dynamic/private";
import { getToken } from "@auth/core/jwt";
import type { RequestHandler } from "./$types";

export const POST: RequestHandler = async ({ cookies, fetch, request }) => {
    let secureCookie = false;
    for (const cookie of cookies.getAll()) {
        if (cookie.name.includes("__Secure-authjs.session-token")) {
            secureCookie = true;
            break;
        }
    }
    const jwt = await getToken({
        req: request,
        secret: env.AUTH_SECRET,
        secureCookie: secureCookie
    });
    const formData = await request.formData();
    const queries = new URLSearchParams({
        "src": formData.get("src")?.toString() ?? "",
        "duration": formData.get("duration")?.toString() ?? ""
    });
    const res = await fetch(`${env.APP_API_URL}/api/auth/stream/token?${queries.toString()}`, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${jwt?.accessToken}`
        }
    });
    if (res.ok) {
        queries.append("token", await res.text());
        return new Response(`${env.APP_URL}/api/${formData.get("type")?.toString()}?${queries.toString()}`);
    } else {
        return new Response(res.statusText, {
            status: res.status,
            statusText: res.statusText
        });
    }
};
