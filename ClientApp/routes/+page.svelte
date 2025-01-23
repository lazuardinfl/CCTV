<script lang="ts">
    import Stream from "$lib/Stream.svelte";

    let cctv = $state();
</script>

<div class="main">
    <input class="cctv" placeholder="CCTV IP/Name" bind:value={cctv}>
    <div class="stream">
        <Stream name="Stream 1" type="stream.mjpeg" {cctv} />
        <Stream name="Stream 2" type="stream.mp4" {cctv} />
        <Stream name="Snapshot 1" type="frame.jpeg" {cctv} />
        <Stream name="Snapshot 2" type="frame.mp4" {cctv} />
    </div>
</div>

<style>
    .main {
        align-items: center;
        display: flex;
        flex-direction: column;
        justify-content: center;
        position: absolute;
        top: 30%;
        width: 100%;
    }

    .cctv {
        border: 2px solid black;
        border-radius: 10px;
        font-family: Verdana, Geneva, Tahoma, sans-serif;
        font-size: 1.2rem;
        font-weight: bold;
        margin-bottom: 30px;
        padding: 10px;
        text-align: center;
        width: 400px;
    }

    .stream {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 25px;
    }
</style>
