<script lang="ts">
    import { base } from "$app/paths";

    let { cctv, name, type } = $props();

    async function getStream() {
        const formData = new FormData();
        formData.append("type", type);
        formData.append("src", cctv);
        formData.append("duration", "normal");
        const res = await fetch(`${base}/stream`, {
            method: "POST",
            body: formData
        });
        if (res.ok) {
            window.open(await res.text(), "_blank");
        } else {
            alert(await res.text());   
        }
    }
</script>

<button class="stream" onclick={getStream}>{name}</button>

<style>
    .stream {
        background-color: white;
        border: 2px solid black;
        border-radius: 20px;
        cursor: pointer;
        font-family: Verdana, Geneva, Tahoma, sans-serif;
        font-size: 1rem;
        font-weight: bold;
        padding: 10px;
        text-align: center;
    }
    .stream:hover {
        background-color: lightcyan;
    }
</style>
