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
    :global(body) {
        background-color: lightgray;
        margin: 0;
    }

    .topnav {
        align-items: center;
        background-color: black;
        display: flex;
        justify-content: space-between;
        padding: 10px 20px;
    }
</style>
