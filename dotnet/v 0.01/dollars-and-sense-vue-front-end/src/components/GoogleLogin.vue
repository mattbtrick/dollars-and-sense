<!--
Say Hello World with Vue!
-->

<script>
  export default{
    mounted() {
      const gClientId = "244218191642-c2lr5r68p0kp20q19esiiocvdeaoqhnh.apps.googleusercontent.com"
      window.google.accounts.id.initialize({
        client_id: gClientId,
        callback: this.handleCredentialResponse,
        auto_select: true
      })
      window.google.accounts.id.renderButton(
        this.$refs.googleLoginBtn, {
        text: 'signin_with', // or 'signup_with' | 'continue_with' | 'signin'
        size: 'large', // or 'small' | 'medium'
        width: '366', // max width 400
        theme: 'outline', // or 'filled_black' |  'filled_blue'
        logo_alignment: 'left' // or 'center'
      }
      )
    },
    methods: {
   async handleCredentialResponse(response) {
        console.log(response.credential)
        // Put your backend code in here
      }
    }
  }
</script>

<template>
  <div ref="googleLoginBtn" />
</template>
