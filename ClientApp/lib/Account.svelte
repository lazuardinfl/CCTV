<script lang="ts">
    import { base } from "$app/paths";
    import { page } from "$app/state";
    import { SignIn, SignOut } from "@auth/sveltekit/components";

    const { signInRoute, signOutRoute } = getAuthRoute();

    function getAuthRoute(): { signInRoute: string, signOutRoute: string } {
        const baseEdit = base.startsWith('/') ? `${base.slice(1)}/` : base;
        return {
            signInRoute: baseEdit + "signin",
            signOutRoute: baseEdit + "signout"
        };
    }
</script>

<div class="account">
    {#if page.data.session?.user}
        <p class="user">{page.data.session.user.name}</p>
        <SignOut signOutPage="{signOutRoute}">
            <div slot="submitButton">
                <span class="auth">Log Out</span>
            </div>
        </SignOut>
    {:else}
        <SignIn provider="keycloak" signInPage="{signInRoute}">
            <div slot="submitButton">
                <span class="auth">Log In</span>
            </div>
        </SignIn>
    {/if}
</div>

<style>
    .account {
        align-items: center;
        display: flex;
        font-family: Verdana, Geneva, Tahoma, sans-serif;
        gap: 20px;
    }

    .auth {
        font-size: large;
        font-weight: bold;
    }

    .user {
        color: white;
        font-size: large;
        margin: 0;
    }
</style>
