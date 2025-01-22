<script lang="ts">
    import { base } from '$app/paths';

    async function getStream() {
        const formData = new FormData();
        formData.append("type", "frame.jpeg");
        formData.append("src", "ip");
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

<button onclick={getStream}>STREAM</button>
