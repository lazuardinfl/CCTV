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
        <p>{page.data.session.user.name}</p>
        <div>
            <SignOut signOutPage="{signOutRoute}" />
        </div>
    {:else}
        <div>
            <SignIn provider="keycloak" signInPage="{signInRoute}" />
        </div>
    {/if}
</div>

<style>
    .account {
        float: right;
    }
</style>
