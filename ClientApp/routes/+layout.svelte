<script lang="ts">
    import Home from "$lib/Home.svelte";
    import Account from "$lib/Account.svelte";
    let { children } = $props();
</script>

<header>
    <Home />
    <Account />
</header>

{@render children()}
