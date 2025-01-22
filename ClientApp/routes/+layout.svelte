<script lang="ts">
    import Account from "$lib/Account.svelte";
    import Home from "$lib/Home.svelte";

    let { children } = $props();
</script>

<header>
    <div class="topnav">
        <Home />
        <Account />
    </div>
</header>

{@render children()}

<style>
    .topnav {
        overflow: hidden;
        background-color: #c1bcbc;
    }
</style>
