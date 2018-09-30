<template lang="html">
  <div class="ui container">
    <div class="ui segment inverted">
      <div class="ui left aligned header">{{ title }}</div>  
    </div>    
  </div>
</template>

<script>  
export default {
  name: 'Header',
  props: [ 'title' ]
}
</script>

<style scoped>
  .ui.container {
    margin-top: 1.5em;
    margin-bottom: 1.5em;
  }
</style>
