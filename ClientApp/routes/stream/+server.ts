import { env } from "$env/dynamic/private";
import { decode } from "@auth/core/jwt";
import type { RequestHandler } from "./$types";

export const POST: RequestHandler = async ({ cookies, fetch, request }) => {
    let authCookie = "__Secure-authjs.session-token";
    if (!cookies.get(authCookie)) {
        authCookie = "authjs.session-token";
    }
    const jwt = await decode({
        salt: authCookie,
        secret: env.AUTH_SECRET,
        token: cookies.get(authCookie)
    });
    const formData = await request.formData();
    const queries = new URLSearchParams({
        "src": formData.get("src")?.toString() ?? "",
        "duration": formData.get("duration")?.toString() ?? ""
    });
    const res = await fetch(`${env.CCTV_API_URL}/api/auth/stream/token?${queries.toString()}`, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${jwt?.accessToken}`
        }
    });
    if (res.ok) {
        queries.append("token", await res.text());
        return new Response(`${env.CCTV_API_CLIENT_URL}/api/${formData.get("type")?.toString()}?${queries.toString()}`);
    } else {
        return new Response(res.statusText, {
            status: res.status,
            statusText: res.statusText
        });
    }
};
